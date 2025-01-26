const CACHE_NAME = 'anduinOS-cache-v28';
const urlsToCache = [
  '/',
  '/index.html',
  '/favicon.svg',
  '/css/site.css',
  '/js/site.js',
  '/sc.webp',
  '/scs/blender.png',
  '/scs/build.png',
  '/scs/office.png',
  '/scs/serve.png',
  '/scs/steam.png',
  '/scs/watch.png',
  '/scs/blender_comp.png',
  '/scs/build_comp.png',
  '/scs/office_comp.png',
  '/scs/serve_comp.png',
  '/scs/steam_comp.png',
  '/scs/watch_comp.png',
  '/ava/aimer.jpg',
  '/ava/yuyuyuyu.jpg',
  '/ava/coderay.png',
  '/node_modules/@aiursoft/uistack/dist/css/app.css',
  '/node_modules/@aiursoft/uistack/dist/js/app.js'
];

const MAX_CACHE_AGE = 7 * 24 * 60 * 60 * 1000; // 7 days in milliseconds

self.addEventListener('install', event => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => {
        console.log('Opened cache');
        return cache.addAll(urlsToCache);
      })
  );
  self.skipWaiting(); // Activate worker immediately
});

self.addEventListener('fetch', event => {
  event.respondWith(
    caches.open(CACHE_NAME).then(cache => {
      return cache.match(event.request).then(cachedResponse => {
        const now = Date.now();

        if (cachedResponse) {
          const cachedTime = cachedResponse.headers.get('sw-cache-time');
          if (cachedTime) {
            const cacheTime = new Date(cachedTime).getTime();
            if (now - cacheTime < MAX_CACHE_AGE) {
              // Return valid cached response
              return cachedResponse;
            }
          }
          // Cache expired, fetch and cache a new version
          return fetchAndCache(event.request, cache);
        } else {
          // No cache, fetch and cache
          return fetchAndCache(event.request, cache);
        }
      });
    })
  );
});

function fetchAndCache(request, cache) {
  return fetch(request).then(networkResponse => {
    if (!networkResponse || networkResponse.status !== 200 || networkResponse.type !== 'basic') {
      return networkResponse;
    }

    const responseClone = networkResponse.clone();

    return responseClone.blob().then(bodyBlob => {
      const cacheHeaders = new Headers(networkResponse.headers);
      cacheHeaders.append('sw-cache-time', new Date().toISOString());

      const responseWithHeaders = new Response(bodyBlob, {
        status: networkResponse.status,
        statusText: networkResponse.statusText,
        headers: cacheHeaders
      });

      cache.put(request, responseWithHeaders);
      return networkResponse;
    });
  });
}


self.addEventListener('activate', event => {
  const cacheWhitelist = [CACHE_NAME];
  event.waitUntil(
    caches.keys().then(cacheNames => {
      return Promise.all(
        cacheNames.map(cacheName => {
          if (!cacheWhitelist.includes(cacheName)) {
            return caches.delete(cacheName);
          }
        })
      );
    })
  );
  self.clients.claim(); // Take control of clients immediately
});
