const CACHE_NAME = 'anduinOS-cache-v15';
const urlsToCache = [
  '/',
  '/index.html',
  '/favicon.svg',
  '/sc.webp',
  '/scs/blender.png',
  '/scs/build.png',
  '/scs/office.png',
  '/scs/serve.png',
  '/scs/steam.png',
  '/scs/watch.png',
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
});

self.addEventListener('fetch', event => {
  event.respondWith(
    caches.match(event.request).then(cachedResponse => {
      if (cachedResponse) {
        // Check if the cache is still valid
        return caches.open(CACHE_NAME).then(cache => {
          return cache.match(event.request).then(response => {
            const cachedTime = new Date(response.headers.get('sw-cache-time')).getTime();
            const now = Date.now();

            if (now - cachedTime > MAX_CACHE_AGE) {
              // Cache is expired, fetch a new version
              return fetchAndCache(event.request, cache);
            }
            return response;
          });
        });
      }

      // If no cache exists, fetch and cache it
      return fetchAndCache(event.request);
    })
  );
});

// Function to fetch and cache a request
function fetchAndCache(request, cache) {
  return fetch(request).then(response => {
    if (!response || response.status !== 200 || response.type !== 'basic') {
      return response;
    }

    const responseToCache = response.clone();
    const cacheHeaders = new Headers(responseToCache.headers);
    cacheHeaders.append('sw-cache-time', new Date().toISOString());

    const responseWithHeaders = new Response(responseToCache.body, {
      status: responseToCache.status,
      statusText: responseToCache.statusText,
      headers: cacheHeaders
    });

    cache.put(request, responseWithHeaders);
    return response;
  });
}

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
