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
        icon: 'https://firebasestorage.googleapis.com/v0/b/tiemkiet-aa7d7.appspot.com/o/Images%2Flogo.png?alt=media&token=c8f55916-0d91-46a5-89ac-7d1b10fa39e8'
    };

    self.registration.showNotification(notificationTitle,
        notificationOptions);
});