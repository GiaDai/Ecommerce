kubectl create namespace logging
kubectl apply -f configmap.yml
kubectl apply -f deployment.yml

kubectl apply -f postgresql-master-pv-pvc.yml
kubectl apply -f postgresql-master-deployment.yml
kubectl apply -f postgresql-slave-deployment.yml
kubectl apply -f postgresql-service.yml
kubectl apply -f haproxy-configmap.yml
kubectl apply -f haproxy-deployment.yml
kubectl apply -f haproxy-service.yml
kubectl apply -f ingress.yml
