READ_ME

this project is a web api allowing a user to add new user with personal details and picture to DB.
DB is CSV file.
When page opens, it loads the list of current users appearing in file (User.csv).
When adding a new user, the user is added to the file and the list of users gets refreshed with the new user.

In order to do the file read/write work and run the project you should add to Web.config the path with the file name (name and path you want for the file to be saved at).
(This is my line in my Web.config file:)
 <add key="UsersFilePath" value="c:\users\keren\desktop\user.csv"/>
If no such file exists it is created.

Dealing with the data (get and post) is done at UsersController.