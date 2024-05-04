# HackerNewsApi

## Features

- Retrieve top `N`, best stories from Hacker News.
- Get item details, including comments, by ID.

## Installation

To use HackerNewsApi in your project, follow these steps:

1. Clone the repository to your local machine.
2. Launch the API and you should land on one and only endpoint for fetch top 10 stories. Change the number as you please to check the top `N` best stories.

## Assumptions

1. API caches responses for particular `N` for 1 minute.
2. API fetches best stories every 1 minute and stores them in `MemoryCache`. To avoid throttling the HackerNews service there is a semaphore that limits the concurrent calls to HackerNews API.
3. No sample integration with telemetry.
4. No Swagger for one endpoint. 
