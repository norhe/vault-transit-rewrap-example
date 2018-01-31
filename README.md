# Vault Transit Rewrap Example

This app demonstrates how to rewrap data when rotating keys with Vault's transit backend.  This requires a database, Vault server, and data to encrypt.

The app will first seed records.  It will then encrypt a selection of fields in each record.  A new version of the transit key will then be created.  Finally, it will select records encrypted with the old key, and rewrap the data using the new key.  

Data encrypted by transit is prepended with the version of the key used.  

## Usage

You need a database to test with.  You can create one easily using Docker:

```
docker pull mysql/mysql-server:5.7
mkdir ~/rewrap-data
docker run --name mysql-rewrap \
  -p 3306:3306 \
  -v ~/rewrap-data:/var/lib/mysql \
  -e MYSQL_ROOT_PASSWORD=root \
  -e MYSQL_ROOT_HOST=% \
  -e MYSQL_DATABASE=my_app \
  -e MYSQL_USER=vault \
  -e MYSQL_PASSWORD=vaultpw \
  -d mysql/mysql-server:5.7
```

To configure Vault:

```
vault server -dev -dev-root-token-id=root &
export VAULT_ADDR='http://127.0.0.1:8200'
vault mount transit
vault write -f transit/keys/my_app_key
```

Please note that the above command runs Vault in dev mode which means that secrets will not be persisted to disk.  If you stop the Vault process you will not be able to read records saved using any keys it created.  You will need to wipe the records from the database, and begin testing with new records.  


REPLACE TEXT
You then need to run the app:

```
VAULT_TOKEN=root go run main.go
```

You can then view the app using a browser at http://localhost:1234.

You can inspect the contents of the database with:
```
docker exec -it mysql-transit mysql -uroot -proot
```