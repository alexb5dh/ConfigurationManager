# WebService

Service provides support for storing and retrieving simple key-value pairs.

# Key format

Key is a simple string consisting of multiple __segments__ separated by "/".  

Each segment can contain digits or letters only. 
This means that only following categories are allowed: UppercaseLetter, LowercaseLetter, TitlecaseLetter, ModifierLetter, OtherLetter, DecimalDigitNumber.  
  
Empty segments are allowed only for Section keys and only if key has following normalized form: "/".

Key are considered equals if they are differing by leading "/", trailing "/" or both only.
__Normalized form__  is used to provide single representation for each key.  
In normalized form key starts with "/" and either is equal to "/" or ends with letter or digit.

Two types of keys are used:  

- __Section__ key is used to query all key-value pairs that are nested under this section.  
  Key is not considered nested under himself.  
  Only section key allows "/" value.  
- __Regular__ key is used to store and retrieve value.  

# Supported Media Types

Currently service support only JSON and will __always__ respond in this format.

# API Methods

## GET value

Gets value at specified key.

### HTTP Methods

__GET__

### Parameters

Name  | Via          | Type
------|--------------| -------
key   | Query string | string

### Returns

Name    | Via           | Type
--------|---------------|--------
value   | Response body | string

### Example

GET http://localhost:8080/value?key=My/Secret

## SET value

Sets value at specified key.

### HTTP Methods

__POST__ or __PUT__

### Parameters

Name  | Via          | Type
------|--------------| -------
value   | Query string or Request body | string

### Returns


### Example

POST http://localhost:8080/value?key=My/Secret  
  
"Nobody will ever find this out."

## GET section

Gets all nested key-value pairs for specified key.

### HTTP Methods

__GET__

### Parameters

Name  | Via          | Type
------|--------------| -------
key   | Query string | string

### Returns

Name    | Via           | Type
--------|---------------|--------
section | Response body | object

Returned object have following format: 

```JSON
{
    "<key1>": <"value1>",
    "<key2>": <"value2>",
    //...
}
```

Each key is contained only once.

### Example

GET http://localhost:8080/section?key=My/

## DELETE value

Deletes value for specified key.

### HTTP Methods

__DELETE__

### Parameters

Name  | Via          | Type
------|--------------| -------
key   | Query string | string

### Returns


### Example

DELETE http://localhost:8080/section?key=My/Secret