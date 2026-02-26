# AxiAI Chat API v3.0 - API Documentation (Updated)

## Overview

This document provides a comprehensive reference for all AxiAI Chat API v3.0 endpoints, including request/response payloads and descriptions.

---

## 📋 API Endpoints Summary

| Endpoint                 | Method | Description                                                                 |
| ------------------------ | ------ | --------------------------------------------------------------------------- |
| `/AxiAI/setapiconfig`    | POST   | Configure or update AI provider settings with encrypted API keys            |
| `/AxiAI/getapiconfig`    | POST   | Retrieve decrypted API key for a specific provider                          |
| `/AxiAI/v2/providers`    | GET    | List all available AI providers                                             |
| `/AxiAI/v2/chat`         | POST   | Send a chat message to any configured AI provider (conversationId optional) |
| `/AxiAI/config/defaults` | GET    | Get the default provider and model configuration                            |

---

## 1. Set API Configuration

### `POST /AxiAI/setapiconfig`

Configures an AI provider with API key and model settings. API keys are automatically encrypted using AES-256 before storage.

**Purpose:** First-time setup, adding new providers, updating API keys, or changing default provider.

#### Request Payload

```json
{
  "provider": "openai",
  "model": "gpt-4o",
  "key": "sk-your-openai-api-key-here",
  "setDefault": true
}
```

| Field        | Type    | Required | Description                                                    |
| ------------ | ------- | -------- | -------------------------------------------------------------- |
| `provider`   | string  | ✅ Yes   | Provider name: `openai`, `gemini`, `claude`, `copilot`         |
| `model`      | string  | ✅ Yes   | Default model for this provider (e.g., `gpt-4o`, `gemini-pro`) |
| `key`        | string  | ✅ Yes   | API key (will be encrypted before storage)                     |
| `setDefault` | boolean | ✅ Yes   | `true` to set as default provider/model, `false` otherwise     |

#### Response Payload (Success)

```json
{
  "success": true,
  "message": "API configuration set for openai",
  "provider": "openai",
  "model": "gpt-4o",
  "isDefault": true
}
```

#### Response Payload (Error)

```json
{
  "success": false,
  "message": "Provider 'invalid-provider' not supported"
}
```

#### Example Usage

```bash
curl -X POST "http://localhost:5001/AxiAI/setapiconfig" \
  -H "Content-Type: application/json" \
  -d '{
    "provider": "openai",
    "model": "gpt-4o",
    "key": "sk-1234567890abcdef",
    "setDefault": true
  }'
```

---

## 2. Get API Configuration (API Key)

### `POST /AxiAI/getapiconfig`

Retrieves the decrypted API key for a specific provider. This endpoint requires the provider name in the request body and returns the plaintext API key.

**Purpose:** Retrieve API key for client-side use, verify key configuration, or for debugging.

#### Request Payload

```json
{
  "provider": "openai"
}
```

| Field      | Type   | Required | Description                           |
| ---------- | ------ | -------- | ------------------------------------- |
| `provider` | string | ✅ Yes   | Provider name to retrieve API key for |

#### Response Payload (Success)

```json
{
  "apikey": "sk-1234567890abcdef"
}
```

| Field    | Type   | Description                                  |
| -------- | ------ | -------------------------------------------- |
| `apikey` | string | Decrypted API key for the requested provider |

#### Response Payload (Error - Provider Not Found)

```json
{
  "error": "Provider 'openai' not found or not configured"
}
```

#### Response Payload (Error - Missing Provider)

```json
{
  "error": "Provider is required"
}
```

#### Example Usage

```bash
curl -X POST "http://localhost:5001/AxiAI/getapiconfig" \
  -H "Content-Type: application/json" \
  -d '{
    "provider": "openai"
  }'
```

---

## 3. Get Available Providers

### `GET /AxiAI/v2/providers`

Returns a list of all AI providers supported by the system, regardless of whether they are configured.

**Purpose:** Discover available providers, check which ones can be used in chat requests.

#### Request Payload

None. This is a GET request with no body.

#### Response Payload

```json
{
  "providers": ["openai", "gemini", "claude", "copilot"],
  "count": 4
}
```

| Field       | Type    | Description                      |
| ----------- | ------- | -------------------------------- |
| `providers` | array   | List of supported provider names |
| `count`     | integer | Number of supported providers    |

#### Example Usage

```bash
curl "http://localhost:5001/AxiAI/v2/providers"
```

---

## 4. Unified Chat

### `POST /AxiAI/v2/chat`

Sends a message to an AI provider and returns the response. This is the primary endpoint for all AI interactions.

**Note:** Conversation history is not yet implemented. The `conversationId` field is optional and will be used in future updates for conversation tracking.

**Purpose:** Core chat functionality with support for multiple providers and models.

#### Request Payload

```json
{
  "userId": "user-123",
  "message": "What is the capital of France?",
  "provider": "openai",
  "model": "gpt-4o",
  "systemPrompt": "You are a helpful geography expert.",
  "additionalParameters": {
    "temperature": 0.7,
    "max_tokens": 500
  }
}
```

| Field                  | Type   | Required | Description                                                         |
| ---------------------- | ------ | -------- | ------------------------------------------------------------------- |
| `userId`               | string | ✅ Yes   | Unique user identifier                                              |
| `message`              | string | ✅ Yes   | User's message to the AI                                            |
| `provider`             | string | ❌ No    | Provider to use (defaults to configured default)                    |
| `model`                | string | ❌ No    | Specific model to use (defaults to provider's default)              |
| `conversationId`       | string | ❌ No    | _Future use_ - Conversation thread identifier (not implemented yet) |
| `systemPrompt`         | string | ❌ No    | Override system prompt for this request                             |
| `additionalParameters` | object | ❌ No    | Provider-specific parameters (temperature, max_tokens, etc.)        |

#### Response Payload (Success)

```json
{
  "userId": "user-123",
  "reply": "The capital of France is Paris.",
  "model": "gpt-4o",
  "provider": "openai",
  "timestamp": "2024-02-13T14:30:45Z",
  "tokensUsed": 45
}
```

| Field            | Type    | Description                                  |
| ---------------- | ------- | -------------------------------------------- |
| `userId`         | string  | Echo of the user ID from request             |
| `reply`          | string  | AI-generated response                        |
| `model`          | string  | Model that generated the response            |
| `provider`       | string  | Provider that handled the request            |
| `timestamp`      | string  | ISO 8601 timestamp of response               |
| `tokensUsed`     | integer | Estimated token count (when available)       |
| `conversationId` | string  | _Future_ - Will be included when implemented |

#### Response Payload (Error)

```json
{
  "error": "Provider 'openai' is not configured. Please configure it first using /setapiconfig",
  "statusCode": 400
}
```

#### Example Usage

```bash
# Using default provider
curl -X POST "http://localhost:5001/AxiAI/v2/chat" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-123",
    "message": "Hello! What can you do?"
  }'

# Specifying provider and model
curl -X POST "http://localhost:5001/AxiAI/v2/chat" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-123",
    "message": "Explain quantum computing",
    "provider": "openai",
    "model": "gpt-4o",
    "additionalParameters": {
      "temperature": 0.5,
      "max_tokens": 1000
    }
  }'

# Including conversationId (for future compatibility)
curl -X POST "http://localhost:5001/AxiAI/v2/chat" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-123",
    "conversationId": "conv-abc-123",
    "message": "What is the capital of France?",
    "provider": "openai"
  }'
```

---

## 5. Get Default Configuration

### `GET /AxiAI/config/defaults`

Returns the currently configured default provider and model.

**Purpose:** Quick lookup of system defaults for client-side decisions or UI display.

#### Request Payload

None. This is a GET request with no body.

#### Response Payload

```json
{
  "provider": "openai",
  "model": "gpt-4o"
}
```

| Field      | Type   | Description                                 |
| ---------- | ------ | ------------------------------------------- |
| `provider` | string | Default provider name                       |
| `model`    | string | Default model name for the default provider |

#### Example Usage

```bash
curl "http://localhost:5001/AxiAI/config/defaults"
```

---

## 🔄 Provider Selection Logic (v2/chat)

When sending a chat request, the system determines which provider/model to use in this order:

1. **If `provider` is specified** → Use that provider
2. **If no `provider`** → Use default provider from configuration
3. **If `model` is specified** → Use that model
4. **If no `model`** → Use default model for the selected provider

---

## 🔐 Security Notes

- **API Key Encryption**: All keys sent to `/setapiconfig` are encrypted using AES-256 before storage
- **Controlled Key Access**: Keys can only be retrieved via `/getapiconfig` with explicit provider request
- **Encryption Prefix**: Stored encrypted values are prefixed with `ENC:` in `appsettings.json`

---

## 📝 Quick Reference Card

| What do you want to do?     | Endpoint           | Method |
| --------------------------- | ------------------ | ------ |
| **First-time setup**        | `/setapiconfig`    | POST   |
| **Add new provider**        | `/setapiconfig`    | POST   |
| **Update API key**          | `/setapiconfig`    | POST   |
| **Change default**          | `/setapiconfig`    | POST   |
| **Retrieve API key**        | `/getapiconfig`    | POST   |
| **See available providers** | `/v2/providers`    | GET    |
| **Send a chat**             | `/v2/chat`         | POST   |
| **Check defaults**          | `/config/defaults` | GET    |

---

**Version:** 3.0.0  
**Last Updated:** February 18, 2026
