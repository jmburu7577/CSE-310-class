from django.contrib import admin
from django.utils.html import format_html
from .models import Event, Booking

# Customize admin site header and title
admin.site.site_header = "Event Booking System Administration"
admin.site.site_title = "Event Booking Admin"
admin.site.index_title = "Welcome to Event Booking System Admin Portal"

# Register your models here.
@admin.register(Event)
class EventAdmin(admin.ModelAdmin):
    list_display = ('name', 'date', 'seats_available', 'booking_count', 'status_badge')
    list_filter = ('date',)
    search_fields = ('name', 'description')
    date_hierarchy = 'date'
    ordering = ('-date',)
    list_per_page = 20
    
    fieldsets = (
        ('Event Information', {
            'fields': ('name', 'description')
        }),
        ('Event Details', {
            'fields': ('date', 'seats_available')
        }),
    )
    
    def booking_count(self, obj):
        """Display the number of bookings for this event"""
        count = obj.bookings.count()
        return count
    booking_count.short_description = 'Total Bookings'
    
    def status_badge(self, obj):
        """Display a colored badge based on seat availability"""
        if obj.seats_available == 0:
            return format_html('<span style="background-color: #dc3545; color: white; padding: 3px 10px; border-radius: 3px;">FULL</span>')
        elif obj.seats_available < 10:
            return format_html('<span style="background-color: #ffc107; color: black; padding: 3px 10px; border-radius: 3px;">LIMITED</span>')
        else:
            return format_html('<span style="background-color: #28a745; color: white; padding: 3px 10px; border-radius: 3px;">AVAILABLE</span>')
    status_badge.short_description = 'Status'

@admin.register(Booking)
class BookingAdmin(admin.ModelAdmin):
    list_display = ('user_name', 'user_email', 'event', 'booking_time', 'event_date')
    list_filter = ('event', 'booking_time')
    search_fields = ('user_name', 'user_email', 'event__name')
    date_hierarchy = 'booking_time'
    readonly_fields = ('booking_time',)
    ordering = ('-booking_time',)
    list_per_page = 25
    
    fieldsets = (
        ('User Information', {
            'fields': ('user_name', 'user_email')
        }),
        ('Booking Details', {
            'fields': ('event', 'booking_time')
        }),
    )
    
    def event_date(self, obj):
        """Display the event date"""
        return obj.event.date.strftime('%Y-%m-%d %H:%M')
    event_date.short_description = 'Event Date'
    event_date.admin_order_field = 'event__date'
    
    actions = ['export_bookings']
    
    def export_bookings(self, request, queryset):
        """Export selected bookings (placeholder for future CSV export)"""
        self.message_user(request, f"{queryset.count()} bookings selected for export.")
    export_bookings.short_description = "Export selected bookings"
