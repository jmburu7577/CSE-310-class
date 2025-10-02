from django.contrib import admin
from .models import Event, Booking

# Register your models here.
@admin.register(Event)
class EventAdmin(admin.ModelAdmin):
    list_display = ('name', 'date', 'seats_available')
    search_fields = ('name',)

@admin.register(Booking)
class BookingAdmin(admin.ModelAdmin):
    list_display = ('user_name', 'user_email', 'event', 'booking_time')
    list_filter = ('event', 'booking_time')
    search_fields = ('user_name', 'user_email')
