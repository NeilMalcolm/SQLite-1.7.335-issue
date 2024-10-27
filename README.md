There is a "not an error" exception message which can be returned when opening/writing to a SQLite database encrypted with SQLCipher, using sqlite-net-sqlcipher 1.7.335.

This was fixed in 1.8.X (see this PR https://github.com/praeclarum/sqlite-net/pull/1026) but 1.8.X has its own issues with iOS when you don't enable the Interpreter.

Using 1.7.335 and doing the following seemed to resolve the issue from a precusrory run through of adding and reading:
1. Adding SQLite.cs from the sqlite-net-pcl project to our code
1. Replacing the problematic lines of code as per the above PR
1. Adding the below constant as per [the sqlite-net-sqlcipher nuget csproj](https://github.com/praeclarum/sqlite-net/blob/master/nuget/SQLite-net-sqlcipher/SQLite-net-sqlcipher.csproj)

```xml
<DefineConstants>USE_SQLITEPCL_RAW</DefineConstants>
```

**Before** adding the fix highlighted in the above Pull Request:

![{5B13590F-F917-4719-B13D-59A362CF4CC8}|50](https://github.com/user-attachments/assets/9944da52-f74f-40e0-b3d5-83edcaf0b534)

**After** adding the fix highlighted in the above Pull Request:

![{514C35F2-E282-4101-B3F1-5B35F8F17DF8}](https://github.com/user-attachments/assets/0068daba-6a98-409e-9abc-eeb0fb4467a8)
