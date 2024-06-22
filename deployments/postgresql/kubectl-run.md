kubectl port-forward service/postgresql-master 5432:5432 -n ecommerce
kubectl port-forward service/postgresql-slave 5435:5432 -n ecommerce

kubectl port-forward service/haproxy-service 5434:5434 -n ecommerce

kubectl create namespace ecommerce
kubectl apply -f secret.yml
kubectl apply -f postgresql-master-pv-pvc.yml
kubectl apply -f postgresql-master-deployment.yml
kubectl apply -f postgresql-slave-deployment.yml
kubectl apply -f postgresql-service.yml
kubectl apply -f haproxy-configmap.yml
kubectl apply -f haproxy-deployment.yml
kubectl apply -f haproxy-service.yml
kubectl apply -f ingress.yml
