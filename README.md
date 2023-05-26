# ReReddit API Client

This program is a Reddit API client that allows users to interact with their Reddit account and perform various actions such as retrieving private messages, managing friends and trusted users, and authenticating via OAuth.

## Controllers

### AccountController

The `AccountController` is responsible for handling API requests related to the user's Reddit account. It provides endpoints for retrieving private messages, managing trusted users and friends, and retrieving their lists.

### CallbackController

The `CallbackController` handles the OAuth callback endpoint. After the user grants authorization on the Reddit website, the callback URL is invoked to exchange the authorization code for an access token. This controller facilitates that exchange process.

## Services

### RedditApiService

The `RedditApiService` class encapsulates the logic for interacting with the Reddit API. It provides methods for retrieving private messages, managing trusted users and friends, and retrieving their lists. It utilizes an HTTP client to send requests to the Reddit API and parse the responses.

### RedditOAuthService

The `RedditOAuthService` handles the OAuth flow for authentication with the Reddit API. It provides methods for generating the authorization URL, exchanging the authorization code for an access token, and refreshing the access token when it expires. This service is used in conjunction with the `CallbackController` for authentication.

## Getting Started

To run this program, make sure you have the necessary dependencies installed and configured. The program requires an HTTP client, such as `HttpClient`, to send requests to the Reddit API. Additionally, you need to set up your Reddit API credentials and configure the OAuth settings.

1. Clone the repository and navigate to the project directory.
2. Configure the Reddit API credentials in the application settings or environment variables.
3. Set up the OAuth settings for the Reddit application, including the client ID, client secret, and redirect URL.
4. Build and run the program.
5. Access the API endpoints provided by the `AccountController` to interact with your Reddit account.

Make sure to handle any necessary authentication and authorization steps before making requests to the protected endpoints.

## Dependencies

- .NET Core (version X.X.X)
- HttpClient (version X.X.X)
- Newtonsoft.Json (version X.X.X) - Used for JSON serialization/deserialization.

## License

This program is released under the [MIT License](LICENSE).
