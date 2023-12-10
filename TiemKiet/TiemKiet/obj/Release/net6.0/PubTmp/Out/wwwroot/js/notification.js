const firebaseConfig = {
    apiKey: "AIzaSyAXK3Zho4-QwdwKowzrmZVRpBbjxBNDtxw",
    authDomain: "leafcoffee-66175.firebaseapp.com",
    projectId: "leafcoffee-66175",
    storageBucket: "leafcoffee-66175.appspot.com",
    messagingSenderId: "189337812695",
    appId: "1:189337812695:web:57b971e4f3acaf508cba92",
    measurementId: "G-MKB2F9R564"
};

const app = firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();

async function getToken() {
    const messaging = firebase.messaging();
    togglePreloader(true);
    try {
        const currentToken = await messaging.getToken({ vapidKey: 'BPPI3HPUZ0xGI4axfOogJXzKwdOM54c_O9zRGwH_ttz8HXJvv3F4FvMv6-bhzuNJp9ljIgxEQHLKQxdJi-JiX2E' });

        if (currentToken) {
            console.log(currentToken);
            return currentToken;
        } else {
            console.log('No registration token available. Request permission to generate one.');
            return "";
        }
    } catch (err) {
        console.log('An error occurred while retrieving token. ', err);
        return "";
    }
    finally {
        togglePreloader(false);
    }
}

function requestPermission() {
    Notification.requestPermission().then((permission) => {
        if (permission === 'granted') {
            console.log('Notification permission granted.');
        } else {
            console.log('Unable to get permission to notify.');
        }
    });
}

requestPermission();

function deleteToken() {
    const messaging = firebase.messaging();

    messaging.deleteToken().then(() => {
        console.log('Token deleted.');
    }).catch((err) => {
        console.log('Unable to delete token. ', err);
    });
}