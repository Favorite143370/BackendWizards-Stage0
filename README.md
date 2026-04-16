# Backend Wizards Stage 0

## API Endpoint
GET /api/classify?name={name}

## Example
https://your-app-url/api/classify?name=john

## Response
{
  "status": "success",
  "data": {
    "name": "john",
    "gender": "male",
    "probability": 0.99,
    "sample_size": 1234,
    "is_confident": true,
    "processed_at": "2026-04-16T12:00:00Z"
  }
}

## Error Example
{
  "status": "error",
  "message": "No prediction available for the provided name"
}

## Tech
- ASP.NET Core Web API
- C#
- Genderize API

## Notes
- CORS enabled
- Handles edge cases
