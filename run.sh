kubectl apply -f . --recursive
sleep 3
kubectl apply -f . --recursive
sleep 1
kubectl config set-context --current --namespace=skor-sever-api-namespace
sleep 1
kubectl get all
sleep 1
kubectl get namespaces
sleep 1
