# Intro
Command line tool for file quantity comparison between AWS S3 Bucket and an Azure Storage Container.

Azure Code contributed by https://github.com/alexandrelbl

# Case
Useful for double checking when migrating a large amount of storage files between the two clouds since a direct sync command line between the two clouds wasn't available up until the creation of this tool.

# Requirements / Build
* .NET Core 3.1 SDK
* AWS credentials at /users/user/.aws/
* dotnet restore
* dotnet build

# Usage
> '.\Compare Files Aws Azure.exe' "bucket-name" "container-name" "storageaccount-name"  "storageaccount-key"

> '.\Compare Files Aws Azure.exe' "bucket-name/directory" "container-name" "storageaccount-name" "storageaccount-key"

# TODO
* Add byte or MD5 checksum comparison to enhance comparison reliability
* Exception handling and argument validation
* Cleaner output
* Move auth args to an external file