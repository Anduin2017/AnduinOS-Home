const CACHE_NAME = 'anduinOS-cache-v10';
const urlsToCache = [
  '/',
  '/index.html',
  '/favicon.svg',
  '/sc.png',
  '/scs/blender.png',
  '/scs/build.png',
  '/scs/office.png',
  '/scs/serve.png',
  '/scs/steam.png',
  '/scs/watch.png',
  '/ava/aimer.jpg',
  '/ava/yuyuyuyu.jpg',
  '/node_modules/aiur-ui-stack/css/app.css',
  '/node_modules/aiur-ui-stack/js/app.js'
];

self.addEventListener('install', event => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => {
        console.log('Opened cache');
        return cache.addAll(urlsToCache);
      })
  );
});

self.addEventListener('fetch', event => {
  event.respondWith(
    caches.match(event.request)
      .then(response => {
        if (response) {
          return response;
        }
        
        return fetch(event.request).then(
          response => {
            if(!response || response.status !== 200 || response.type !== 'basic') {
              return response;
            }

            const responseToCache = response.clone();
            caches.open(CACHE_NAME)
              .then(cache => {
                cache.put(event.request, responseToCache);
              });

            return response;
          }
        );
      })
  );
});

self.addEventListener('activate', event => {
  const cacheWhitelist = [CACHE_NAME];
  event.waitUntil(
    caches.keys().then(cacheNames => {
      return Promise.all(
        cacheNames.map(cacheName => {
          if (cacheWhitelist.indexOf(cacheName) === -1) {
            return caches.delete(cacheName);
          }
        })
      );
    })
  );
});
