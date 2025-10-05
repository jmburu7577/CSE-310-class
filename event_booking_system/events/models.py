from django.db import models
from django.utils import timezone
from django.core.validators import MinValueValidator
from django.core.exceptions import ValidationError

# Create your models here.
class Event(models.Model):
    """
    Event model to store information about events
    Fields:
    - name: Name of the event
    - description: Detailed description of the event
    - date: Date and time when the event will take place
    - seats_available: Number of available seats for the event
    - created_at: Timestamp when the event was created
    - updated_at: Timestamp when the event was last updated
    """
    name = models.CharField(max_length=200, help_text="Enter the event name")
    description = models.TextField(help_text="Provide a detailed description of the event")
    date = models.DateTimeField(help_text="Event date and time")
    seats_available = models.PositiveIntegerField(
        validators=[MinValueValidator(0)],
        help_text="Number of seats available for booking"
    )
    created_at = models.DateTimeField(auto_now_add=True, null=True, blank=True)
    updated_at = models.DateTimeField(auto_now=True, null=True, blank=True)
    
    class Meta:
        ordering = ['-date']
        verbose_name = 'Event'
        verbose_name_plural = 'Events'
    
    def __str__(self):
        return self.name
    
    def clean(self):
        """Validate that the event date is in the future"""
        if self.date and self.date < timezone.now():
            raise ValidationError({'date': 'Event date must be in the future.'})
    
    def is_fully_booked(self):
        """Check if the event is fully booked"""
        return self.seats_available == 0
    
    def get_booking_percentage(self):
        """Calculate the percentage of seats booked"""
        total_bookings = self.bookings.count()
        if total_bookings == 0:
            return 0
        total_seats = total_bookings + self.seats_available
        return (total_bookings / total_seats) * 100

class Booking(models.Model):
    """
    Booking model to store information about bookings
    Fields:
    - event: Foreign key to the Event model
    - user_name: Name of the user making the booking
    - user_email: Email of the user making the booking
    - booking_time: Time when the booking was made (auto-set)
    - num_seats: Number of seats booked (default: 1)
    """
    event = models.ForeignKey(Event, on_delete=models.CASCADE, related_name='bookings')
    user_name = models.CharField(max_length=100, help_text="Full name of the attendee")
    user_email = models.EmailField(help_text="Email address for booking confirmation")
    booking_time = models.DateTimeField(default=timezone.now)
    num_seats = models.PositiveIntegerField(
        default=1,
        validators=[MinValueValidator(1)],
        help_text="Number of seats to book"
    )
    
    class Meta:
        ordering = ['-booking_time']
        verbose_name = 'Booking'
        verbose_name_plural = 'Bookings'
        unique_together = [['event', 'user_email']]  # Prevent duplicate bookings
    
    def __str__(self):
        return f"{self.user_name} - {self.event.name}"
    
    def clean(self):
        """Validate booking constraints"""
        if self.event and self.event.is_fully_booked():
            raise ValidationError('This event is fully booked.')
        
        if self.num_seats and self.event and self.num_seats > self.event.seats_available:
            raise ValidationError(f'Only {self.event.seats_available} seats available.')
