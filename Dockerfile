FROM hub.aiursoft.cn/node:21-alpine AS npm-env

WORKDIR /app
COPY ./src .

RUN npm install --loglevel verbose

# ============================
# Prepare Runtime Environment
FROM hub.aiursoft.cn/aiursoft/static
COPY --from=npm-env /app /data

EXPOSE 5000
