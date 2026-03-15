# Acompanhar

# Dicas:

## Executar banco de dados com docker:

```
docker run -d --name acompanhar-db -e POSTGRES_DB=AcompanharDb -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=123 -p 5432:5432 postgres:latest
```