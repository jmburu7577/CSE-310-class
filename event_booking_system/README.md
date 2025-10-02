# Event Booking System

A Django-based web application for managing event bookings. This project was developed as part of the CSE-310 Web Apps Module assignment.

## Project Description

The Event Booking System allows users to:
- View a list of upcoming events
- See detailed information about each event
- Book available seats for events
- Receive booking confirmations

The application demonstrates core web development concepts including:
- Dynamic content generation
- Database integration
- Form handling and validation
- User interaction

## Features

- **Home Page**: Displays a list of upcoming events dynamically from the database
- **Event Detail Page**: Shows event details and provides a booking form
- **Booking Confirmation Page**: Displays confirmation with event and user booking information
- **Database Integration**: Uses SQLite to store event and booking information
- **Responsive Design**: Built with Bootstrap for a mobile-friendly experience

## Installation & Setup

### Prerequisites

- Python 3.8 or higher
- Django 5.2 or higher

### Installation Steps

1. Clone the repository or download the project files

2. Install Django:
   ```
   pip install django
   ```

3. Navigate to the project directory:
   ```
   cd event_booking_system
   ```

4. Apply database migrations:
   ```
   python manage.py migrate
   ```

5. Create a superuser (for admin access):
   ```
   python manage.py createsuperuser
   ```

6. Run the development server:
   ```
   python manage.py runserver
   ```

7. Access the application at: http://127.0.0.1:8000/

## Usage Instructions

### Admin Interface

1. Access the admin interface at: http://127.0.0.1:8000/admin/
2. Log in with the superuser credentials created during setup
3. Add, edit, or delete events through the admin interface

### User Interface

1. Browse the list of upcoming events on the home page
2. Click on an event to view its details
3. Fill out the booking form with your name and email
4. Submit the form to book a seat
5. View the booking confirmation page

## Project Structure

- `events/models.py`: Contains the Event and Booking models
- `events/views.py`: Contains the views for the home page, event detail, and booking confirmation
- `events/templates/`: Contains the HTML templates for the application
- `events/urls.py`: Contains the URL patterns for the application

## Demo Video

[Link to Demo Video](https://example.com/demo-video) (Placeholder for demo video link)

## Technologies Used

- Django 5.2
- Bootstrap 5.3
- SQLite (Database)
- HTML/CSS/JavaScript

## License

This project is created for educational purposes as part of the CSE-310 Web Apps Module assignment.