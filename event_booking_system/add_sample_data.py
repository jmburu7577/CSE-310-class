import os
import django
import datetime
from django.utils import timezone

# Set up Django environment
os.environ.setdefault('DJANGO_SETTINGS_MODULE', 'event_booking_system.settings')
django.setup()

# Import models
from events.models import Event, Booking

def create_sample_events():
    # Clear existing events
    Event.objects.all().delete()
    
    # Create sample events
    events = [
        {
            'name': 'Tech Conference 2023',
            'description': 'Join us for the biggest tech conference of the year! Learn about the latest technologies, network with industry professionals, and participate in hands-on workshops.',
            'date': timezone.now() + datetime.timedelta(days=30),
            'seats_available': 100
        },
        {
            'name': 'Music Festival',
            'description': 'A weekend of amazing music performances from top artists across multiple genres. Food, drinks, and entertainment for all ages.',
            'date': timezone.now() + datetime.timedelta(days=45),
            'seats_available': 500
        },
        {
            'name': 'Web Development Workshop',
            'description': 'Learn the fundamentals of web development in this hands-on workshop. Topics include HTML, CSS, JavaScript, and modern frameworks.',
            'date': timezone.now() + datetime.timedelta(days=15),
            'seats_available': 30
        },
        {
            'name': 'Startup Networking Event',
            'description': 'Connect with entrepreneurs, investors, and industry experts. Perfect opportunity to pitch your ideas and find potential collaborators.',
            'date': timezone.now() + datetime.timedelta(days=7),
            'seats_available': 50
        },
        {
            'name': 'Data Science Bootcamp',
            'description': 'Intensive 3-day bootcamp covering data analysis, machine learning, and AI applications. Suitable for beginners and intermediate practitioners.',
            'date': timezone.now() + datetime.timedelta(days=60),
            'seats_available': 25
        },
    ]
    
    # Save events to database
    for event_data in events:
        event = Event(**event_data)
        event.save()
        print(f"Created event: {event.name}")

def main():
    print("Adding sample data to the database...")
    create_sample_events()
    print("Sample data added successfully!")

if __name__ == "__main__":
    main()