importScripts('https://www.gstatic.com/firebasejs/8.10.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.10.1/firebase-messaging.js');

firebase.initializeApp({
    apiKey: "AIzaSyAXK3Zho4-QwdwKowzrmZVRpBbjxBNDtxw",
    authDomain: "leafcoffee-66175.firebaseapp.com",
    projectId: "leafcoffee-66175",
    storageBucket: "leafcoffee-66175.appspot.com",
    messagingSenderId: "189337812695",
    appId: "1:189337812695:web:57b971e4f3acaf508cba92",
    measurementId: "G-MKB2F9R564",
});
const messaging = firebase.messaging();

messaging.setBackgroundMessageHandler(function (payload) {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    // Customize notification here
    const notificationTitle = 'Background Message Title';
    const notificationOptions = {
        body: 'Background Message body.',
        icon: '/firebase-logo.png'
    };

    self.registration.showNotification(notificationTitle,
        notificationOptions);
});