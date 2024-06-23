docker build -f Ecommerce/Ecommerce.WebApp.Server/Dockerfile -t be-ecommerce:lastest .

echo -n 'Host=postgresql-master;Database=ecommerce;Username=postgres;Password=postgres' | openssl base64
echo -n 'cloudinary://853652539285151:35dgbXUNu7U4_zye8KiQkR5GagA@hqxqmqmoo' | openssl base64

kubectl apply -f secret.yml
kubectl apply -f configmap.yml
kubectl apply -f deployment.yml
kubectl apply -f service.yml
kubectl apply -f ingress.yml

kubectl logs pod/be-ecommerce-84bfdbb5f5-jfd6t -n ecommerce