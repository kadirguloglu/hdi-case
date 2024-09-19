docker compose -f "docker-compose.yml" up -d --build
open http://localhost:7050/api/swagger/index.html?urls.primaryName=Api%20V1.0
open http://localhost:3000
cd FrontEnd/hdi-case && npm install && npm run dev