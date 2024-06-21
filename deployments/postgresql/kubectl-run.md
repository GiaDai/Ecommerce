kubectl port-forward service/postgresql-master 5432:5432 -n ecommerce

kubectl port-forward service/haproxy-service 5432:5435 -n ecommerce
