docker build -t fe.ecommerce:v.0 -f Ecommerce/Ecommerce.WebApp.FrontEnd/Ecommerce.WebApp.FrontEnd.Server/Dockerfile .
docker run -p 8080:8080 --name=fe.ecommerce fe.ecommerce:v.0
