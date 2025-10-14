# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

Repository overview
- This repo contains two unrelated subprojects:
  - event_booking_system: A Django 5.2 web app for browsing events and booking seats. Primary app is events with models (Event, Booking), views, URLs, and a helper script to add sample data.
  - EmployeeManagement: A minimal .NET 8 console project (no tests found).

Common development commands

Python/Django: event_booking_system
- Prereqs: Python 3.8+ and Django 5.2+ (per event_booking_system/README.md)
- Initial setup (from repo root):
  ```powershell path=null start=null
  # install Django (per README)
  pip install django

  # work inside the Django project directory
  Set-Location .\event_booking_system

  # create/update the SQLite schema
  python manage.py migrate
  ```
- Run the dev server:
  ```powershell path=null start=null
  python manage.py runserver
  ```
- Create an admin user:
  ```powershell path=null start=null
  python manage.py createsuperuser
  ```
- Run all tests:
  ```powershell path=null start=null
  python manage.py test
  ```
- Run a single test (replace with your dotted path):
  ```powershell path=null start=null
  python manage.py test events.tests:MyTestCase.test_specific_behavior
  ```
- Framework checks (useful for quick validations):
  ```powershell path=null start=null
  python manage.py check
  ```
- Add sample data (creates several future-dated events):
  ```powershell path=null start=null
  # must run from within the project directory so DJANGO_SETTINGS_MODULE resolves
  Set-Location .\event_booking_system
  python .\add_sample_data.py
  ```
- Database: SQLite at event_booking_system/db.sqlite3 is created after migrate.

C#/.NET: EmployeeManagement
- Build the console project:
  ```powershell path=null start=null
  dotnet build .\EmployeeManagement\EmployeeManagement.csproj
  ```
- Run the app:
  ```powershell path=null start=null
  dotnet run --project .\EmployeeManagement\EmployeeManagement.csproj
  ```
- Tests and linting: No test projects or analyzer configs were found in this repo.

High-level architecture and structure

Django event_booking_system
- Project wiring
  - event_booking_system/event_booking_system/urls.py includes the events app URLs at the root path. Admin is exposed under /admin/.
  - settings.py enables INSTALLED_APPS with events and uses SQLite by default. TEMPLATES uses app directories.
- Domain models (events/models.py)
  - Event: name, description, date (must be in the future), seats_available (>= 0), with convenience helpers:
    - is_fully_booked() and get_booking_percentage().
  - Booking: links to Event with user_name, user_email, num_seats (>= 1), booking_time default now; unique_together on (event, user_email) prevents duplicate bookings per email.
- Request flow (events)
  - URLs (events/urls.py):
    - "" (home) → views.home
    - "event/<id>/" → views.event_detail
    - "event/<id>/book/" → views.book_event
    - "booking/<id>/confirmation/" → views.booking_confirmation
  - Views (events/views.py):
    - home: Lists upcoming events (date >= now), optional search against name/description.
    - event_detail: Shows an event, recent bookings, a BookingForm, and booking percentage.
    - book_event: Validates POSTed fields, prevents duplicates by email, ensures capacity, creates Booking, decrements Event.seats_available, then redirects to confirmation. Uses Django messages for user feedback.
    - booking_confirmation: Displays booking details.
  - Forms: BookingForm is a ModelForm for Booking with basic Bootstrap-friendly widgets; booking-time constraints are enforced at the view/model layer.
- Admin and templates
  - Admin is enabled (django.contrib.admin). The README instructs you to create a superuser and manage events through the admin UI.
  - Templates are referenced in views (e.g., events/home.html, events/event_detail.html, events/booking_confirmation.html) and are resolved via app template directories.
- Data seeding
  - add_sample_data.py sets DJANGO_SETTINGS_MODULE, initializes Django, clears existing Event rows, and seeds several future events. Run it from the event_booking_system directory.

.NET EmployeeManagement
- Project file targets net8.0 with nullable and implicit usings enabled. No further structure is present in this repo.

Notes specific to this repo
- No Python requirements/constraints files were found; installation follows the event_booking_system/README.md guidance (pip install django).
- No linting/formatting configs (Python or .NET) or AI rules files (Claude, Cursor, Copilot) were found.
- The majority of active code and development workflows live under event_booking_system; EmployeeManagement is standalone and minimal.
