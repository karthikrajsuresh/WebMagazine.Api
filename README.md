# WebMagazine.Api

## Overview

The `WebMagazine.Api` project is a comprehensive ASP.NET Core API application designed to power an online magazine platform. It provides a robust backend for managing and delivering content, users, and interactions. This API supports functionalities such as content management, user management, comment handling, notifications, and more, ensuring a dynamic and engaging online magazine experience.

## Features

- **User Management:** Handles user registration, authentication with JWT, and profile management.
- **Content Management:** Supports CRUD operations for articles, categories, tags, and comments, including nested comments.
- **Image Handling:** Allows uploading and storing images in Base64 format directly within the database.
- **Notifications:** Sends notifications to users about content updates and interactions.
- **Role Management:** Admins can manage user roles and permissions with fine-grained access control.
- **Logging:** Implements advanced logging using Serilog for monitoring and troubleshooting.
- **Error Handling:** Includes robust error handling with clear status codes and detailed messages.
- **API Versioning:** Supports versioning to ensure backward compatibility as the API evolves.

## Architecture

The project follows a clean architecture with a focus on separation of concerns. It employs:

- **ASP.NET Core** for building RESTful APIs.
- **Entity Framework Core** for database interactions.
- **AutoMapper** for object-to-object mapping.
- **Serilog** for advanced logging capabilities.
- **JWT Bearer Tokens** for secure authentication and authorization.
- **API Versioning** to manage and evolve the API over time.

## Endpoints

- **Users:** `/api/v{version}/users` - Manage user accounts and profiles.
- **Articles:** `/api/v{version}/articles` - Create, retrieve, update, delete articles, and handle image uploads.
- **Categories:** `/api/v{version}/categories` - Manage content categories.
- **Tags:** `/api/v{version}/tags` - Manage content tags.
- **Comments:** `/api/v{version}/comments` - Manage comments on articles, including nested comments.
- **Notifications:** `/api/v{version}/notifications` - Manage user notifications.
