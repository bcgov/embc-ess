# matches the node version used by openshift S2i image
FROM node:8.11.4-alpine as node

WORKDIR /usr/src/app

# install a specific NPM version
RUN npm install -g npm@6.9.0

COPY package*.json ./

RUN npm install

COPY . .

# ng serve (4200)
# webpack hot module reload (49153)
EXPOSE 4200 49153

CMD ["npm", "start", "--", "--poll=500"]
