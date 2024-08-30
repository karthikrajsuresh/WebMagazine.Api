
# WebMagazine.Api

## Overview

The `WebMagazine.Api` project is a comprehensive .NET Core API application designed to power an online magazine platform. It offers a robust backend for managing and delivering content, users, and interactions. This API provides functionalities for content management, user management, subscription services, notifications, and more, supporting a dynamic and engaging online magazine experience.

## Features

- **User Management:** Handles user registration, authentication, and profile management.
- **Content Management:** Supports CRUD operations for articles, categories, tags, and comments.
- **Subscription Management:** Manages user subscriptions to categories, authors, or topics.
- **Notifications:** Sends notifications to users for content updates and interactions.
- **Role Management:** Admins can manage user roles and permissions.
- **Logging:** Implements custom logging for monitoring and troubleshooting.
- **Error Handling:** Includes robust error handling with clear status codes and messages.

## Architecture

The project follows a clean architecture with a focus on separation of concerns. It employs:

- **ASP.NET Core** for building RESTful APIs.
- **Entity Framework Core** for database interactions.
- **AutoMapper** for object-to-object mapping.
- **Serilog** for advanced logging capabilities.
- **JWT Bearer Tokens** for authentication and authorization.

## Endpoints

- **Users:** `/api/users` - Manage user accounts and profiles.
- **Articles:** `/api/articles` - Create, retrieve, update, and delete articles.
- **Categories:** `/api/categories` - Manage content categories.
- **Tags:** `/api/tags` - Manage content tags.
- **Comments:** `/api/comments` - Manage comments on articles.
- **Subscriptions:** `/api/subscriptions` - Handle user subscriptions.
- **Notifications:** `/api/notifications` - Manage notifications for users.
