from django.shortcuts import render, get_object_or_404, redirect
from django.utils import timezone
from .models import Event, Booking
from django import forms

# Create your views here.

class BookingForm(forms.Form):
    """
    Form for booking an event
    Fields:
    - user_name: Name of the user making the booking
    - user_email: Email of the user making the booking
    """
    user_name = forms.CharField(max_length=100, widget=forms.TextInput(attrs={'class': 'form-control'}))
    user_email = forms.EmailField(widget=forms.EmailInput(attrs={'class': 'form-control'}))

def home(request):
    """
    View for the home page
    Displays a list of upcoming events
    """
    # Get all events that are in the future
    upcoming_events = Event.objects.filter(date__gte=timezone.now()).order_by('date')
    return render(request, 'events/home.html', {'events': upcoming_events})

def event_detail(request, event_id):
    """
    View for the event detail page
    Displays details of a specific event and provides a form for booking
    """
    event = get_object_or_404(Event, pk=event_id)
    form = BookingForm()
    return render(request, 'events/event_detail.html', {'event': event, 'form': form})

def book_event(request, event_id):
    """
    View for booking an event
    Processes the booking form and creates a new booking
    """
    event = get_object_or_404(Event, pk=event_id)
    
    if request.method == 'POST':
        form = BookingForm(request.POST)
        if form.is_valid():
            # Check if there are seats available
            if event.seats_available > 0:
                # Create a new booking
                booking = Booking(
                    event=event,
                    user_name=form.cleaned_data['user_name'],
                    user_email=form.cleaned_data['user_email']
                )
                booking.save()
                
                # Reduce the number of available seats
                event.seats_available -= 1
                event.save()
                
                # Redirect to the booking confirmation page
                return redirect('booking_confirmation', booking_id=booking.id)
            else:
                # No seats available
                form.add_error(None, 'Sorry, this event is fully booked.')
    else:
        form = BookingForm()
    
    return render(request, 'events/event_detail.html', {'event': event, 'form': form})

def booking_confirmation(request, booking_id):
    """
    View for the booking confirmation page
    Displays a confirmation message with event details and user booking information
    """
    booking = get_object_or_404(Booking, pk=booking_id)
    return render(request, 'events/booking_confirmation.html', {'booking': booking})
