kubectl create namespace hdi-case-api-namespace
sleep 10
helm repo add bitnami https://charts.bitnami.com/bitnami
sleep 10
helm repo add kedacore https://kedacore.github.io/charts
sleep 10
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
sleep 10
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
sleep 10
helm repo add datalust https://helm.datalust.co
sleep 10
helm repo update
sleep 10
helm install rabbitmq-cluster bitnami/rabbitmq --namespace rabbitmq-namespace --create-namespace -f helm/rabbitmq.yaml #rabbitmq kurumu
sleep 10
helm install redis-cluster bitnami/redis --namespace redis-namespace --create-namespace --set cluster.enabled=true -f helm/redis.yaml #redis kurulumu
sleep 10
helm install keda kedacore/keda --namespace keda-namespace --create-namespace #rabbitmq scale icin keda kurulumu
sleep 10
helm install ingress-nginx ingress-nginx/ingress-nginx --namespace ingress-nginx --create-namespace -f helm/ingress.yaml #ingress kurulumu
sleep 10
helm install seq datalust/seq --namespace seq-namespace --create-namespace #logging icin seq kurulumu
sleep 10
sh run.sh