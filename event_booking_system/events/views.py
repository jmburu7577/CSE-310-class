from django.shortcuts import render, get_object_or_404, redirect
from django.utils import timezone
from django.contrib import messages
from django.db.models import Q
from django.core.exceptions import ValidationError
from .models import Event, Booking
from django import forms

# Create your views here.

class BookingForm(forms.ModelForm):
    """
    Form for booking an event
    Fields:
    - user_name: Name of the user making the booking
    - user_email: Email of the user making the booking
    - num_seats: Number of seats to book
    """
    class Meta:
        model = Booking
        fields = ['user_name', 'user_email', 'num_seats']
        widgets = {
            'user_name': forms.TextInput(attrs={
                'class': 'form-control',
                'placeholder': 'Enter your full name'
            }),
            'user_email': forms.EmailInput(attrs={
                'class': 'form-control',
                'placeholder': 'Enter your email address'
            }),
            'num_seats': forms.NumberInput(attrs={
                'class': 'form-control',
                'min': '1',
                'value': '1'
            })
        }

def home(request):
    """
    View for the home page
    Displays a list of upcoming events with search functionality
    """
    # Get search query from request
    search_query = request.GET.get('search', '')
    
    # Get all events that are in the future
    upcoming_events = Event.objects.filter(date__gte=timezone.now())
    
    # Apply search filter if query exists
    if search_query:
        upcoming_events = upcoming_events.filter(
            Q(name__icontains=search_query) | 
            Q(description__icontains=search_query)
        )
    
    upcoming_events = upcoming_events.order_by('date')
    
    context = {
        'events': upcoming_events,
        'search_query': search_query,
        'total_events': upcoming_events.count()
    }
    
    return render(request, 'events/home.html', context)

def event_detail(request, event_id):
    """
    View for the event detail page
    Displays details of a specific event and provides a form for booking
    """
    event = get_object_or_404(Event, pk=event_id)
    form = BookingForm()
    
    # Get existing bookings for this event
    recent_bookings = event.bookings.all()[:5]
    
    context = {
        'event': event,
        'form': form,
        'recent_bookings': recent_bookings,
        'booking_percentage': event.get_booking_percentage()
    }
    
    return render(request, 'events/event_detail.html', context)

def book_event(request, event_id):
    """
    View for booking an event
    Processes the booking form and creates a new booking
    """
    event = get_object_or_404(Event, pk=event_id)
    
    if request.method == 'POST':
        # Validate form fields only (without model validation)
        user_name = request.POST.get('user_name', '').strip()
        user_email = request.POST.get('user_email', '').strip()
        try:
            num_seats = int(request.POST.get('num_seats', 1))
        except (ValueError, TypeError):
            num_seats = 1
        
        # Basic validation
        if not user_name or not user_email:
            messages.error(request, 'Please provide both name and email.')
            return redirect('event_detail', event_id=event.id)
        
        if num_seats < 1:
            messages.error(request, 'Number of seats must be at least 1.')
            return redirect('event_detail', event_id=event.id)
        
        # Check if user already has a booking for this event
        existing_booking = Booking.objects.filter(
            event=event,
            user_email=user_email
        ).first()
        
        if existing_booking:
            messages.error(request, 'You have already booked this event.')
            return redirect('event_detail', event_id=event.id)
        
        # Check if there are enough seats available
        if event.seats_available >= num_seats:
            try:
                # Create a new booking
                booking = Booking(
                    event=event,
                    user_name=user_name,
                    user_email=user_email,
                    num_seats=num_seats
                )
                booking.save()
                
                # Reduce the number of available seats
                event.seats_available -= num_seats
                event.save()
                
                messages.success(request, f'Successfully booked {num_seats} seat(s) for {event.name}!')
                
                # Redirect to the booking confirmation page
                return redirect('booking_confirmation', booking_id=booking.id)
            except Exception as e:
                messages.error(request, f'Error creating booking: {str(e)}')
        else:
            messages.error(request, f'Sorry, only {event.seats_available} seat(s) available.')
    
    form = BookingForm()
    return render(request, 'events/event_detail.html', {'event': event, 'form': form})

def booking_confirmation(request, booking_id):
    """
    View for the booking confirmation page
    Displays a confirmation message with event details and user booking information
    """
    booking = get_object_or_404(Booking, pk=booking_id)
    return render(request, 'events/booking_confirmation.html', {'booking': booking})
