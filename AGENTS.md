# MediatR Commands and Queries

This document describes the MediatR command and query handlers in this application.

## Commands

### AddJournalCommand
- **Handler**: AddJournalCommandHandler
- **Description**: Creates a new journal entry
- **Properties**: Name, Issn

### UpdateJournalCommand
- **Handler**: UpdateJournalCommandHandler
- **Description**: Updates an existing journal entry
- **Properties**: Id, Name, Aimscope

### DeleteJournalCommand
- **Handler**: DeleteJournalCommandHandler
- **Description**: Deletes a journal entry
- **Properties**: Id

### CreateUserCommand
- **Handler**: CreateUserCommandHandler
- **Description**: Creates a new user
- **Properties**: (none specified)

## Queries

### GetByIdQuery
- **Handler**: GetByIdQueryHandler
- **Description**: Retrieves a journal by its ID
- **Properties**: Id

### GetJournalsQuery
- **Handler**: GetJournalsQueryHandler
- **Description**: Retrieves all journals
- **Properties**: Search, PageNumber, PageSize