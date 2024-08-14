const CACHE_NAME = 'anduinOS-cache-v1';
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

// 安装 Service Worker 并缓存资源
self.addEventListener('install', event => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => {
        console.log('Opened cache');
        return cache.addAll(urlsToCache);
      })
  );
});

// 处理网络请求，优先从缓存中获取资源
self.addEventListener('fetch', event => {
  event.respondWith(
    caches.match(event.request)
      .then(response => {
        // 如果资源在缓存中，直接返回
        if (response) {
          return response;
        }
        
        // 如果资源不在缓存中，进行网络请求
        return fetch(event.request).then(
          response => {
            // 如果请求失败，返回错误
            if(!response || response.status !== 200 || response.type !== 'basic') {
              return response;
            }

            // 将新的资源添加到缓存中
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

// 更新 Service Worker，删除旧缓存
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
