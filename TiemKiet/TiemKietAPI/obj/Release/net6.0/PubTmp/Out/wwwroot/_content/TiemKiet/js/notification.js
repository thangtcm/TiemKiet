const firebaseConfig = {
    apiKey: "AIzaSyAXK3Zho4-QwdwKowzrmZVRpBbjxBNDtxw",
    authDomain: "leafcoffee-66175.firebaseapp.com",
    projectId: "leafcoffee-66175",
    storageBucket: "leafcoffee-66175.appspot.com",
    messagingSenderId: "189337812695",
    appId: "1:189337812695:web:57b971e4f3acaf508cba92",
    measurementId: "G-MKB2F9R564"
};

firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();

function registerFCMToken() {
    messaging.requestPermission().then(() => {
        return messaging.getToken();
    }).then((token) => {
        console.log('FCM Token:', token);
        // Gửi token đến máy chủ để lưu trữ (backend của bạn)
    }).catch((error) => {
        console.error('Error getting FCM token:', error);
    });
}

messaging.onMessage((payload) => {
    console.log('Message received:', payload);
    // Xử lý thông báo và hiển thị thông báo với jQuery (hoặc DOM manipulation)
    $('#notification').text(payload.notification.title + ': ' + payload.notification.body);
});