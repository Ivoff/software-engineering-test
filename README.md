# About
This application was intended to be a simplistic copy of something like reddit. Users have accounts, accounts can post and create forums. Forums hold posts and are intended to gather posts of some interest or topic in especific. In the initial concept, comments, moderators and blacklisted users in forums but I was not able to finish it everything until the deadline, and many features were stripped. 

The app domain was designed following DDD, it exposes as REST web API and the frontend was made using angular.

----

## Requirements
* Docker
* Docker Compose

## How to execute
```
softwareengineeringtest/ $ docker-compose up
```

## TODO
* Implement Unit of Work and rollback
* Make use of config files
* Study the use of asynchronous procedures
* Improve user session handling in the frontend
* Pagination or lazyload