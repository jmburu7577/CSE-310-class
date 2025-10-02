from django.db import models
from django.utils import timezone

# Create your models here.
class Event(models.Model):
    """
    Event model to store information about events
    Fields:
    - name: Name of the event
    - description: Detailed description of the event
    - date: Date and time when the event will take place
    - seats_available: Number of available seats for the event
    """
    name = models.CharField(max_length=200)
    description = models.TextField()
    date = models.DateTimeField()
    seats_available = models.PositiveIntegerField()
    
    def __str__(self):
        return self.name

class Booking(models.Model):
    """
    Booking model to store information about bookings
    Fields:
    - event: Foreign key to the Event model
    - user_name: Name of the user making the booking
    - user_email: Email of the user making the booking
    - booking_time: Time when the booking was made (auto-set)
    """
    event = models.ForeignKey(Event, on_delete=models.CASCADE, related_name='bookings')
    user_name = models.CharField(max_length=100)
    user_email = models.EmailField()
    booking_time = models.DateTimeField(default=timezone.now)
    
    def __str__(self):
        return f"{self.user_name} - {self.event.name}"
