# Ozet

Proje'nin docker configurasyonlari yapilmistir. init.sh dosyasi calistirilarak tum servisler ayaga kaldirilabilir.
Backend servisi http1 uzerinde 7050 portunda calismaktadir. Bu portun erisilebilir oldugundan emin olunuz.
Backend servisi http2 uzerinde 7060 portunda calismaktadir. Bu portun erisilebilir oldugundan emin olunuz.
Frontend projesi Vite-ReactTS kullanilarak gelistirilmistir. 3000 portunda calismaktadir. Bu portun erisilebilir oldugundan emin olunuz.

# Port degisimleri nasil yapilir?

Backend servislerinin port degisinleri icin RestApi projesinin program.cs dosyasinda bulunan port numaralari degistirilebilir.
Tanimlanan yeni port numaralarini Dockerfile.dev, Dockerfile.prod, docker-compose.yml ve frontend/hdi-case klasorunde bulunan env dosyalarindada yapilmalidir.

Frontend projesinin port degisimi icin frontend/hdi-case/package.json dosyasinda bulunan script'ten yapabilirsiniz.

# Kubernetes uzerinde yayinlama

Kubernetes cluster'inize baglandiktan sonra helm-install-clusters.sh dosyasini 1 kere calistirilarak tum kurulumlar yapilir.
bu dosya kurulumlardan sonra ruh.sh dosyasini calistiracaktir. run.sh dosyasinda kubernetes icin yazilan tum yml dosyalari calistirilacak ve servisiniz
kubernetes uzerinde kosmaya baslayacaktir.
