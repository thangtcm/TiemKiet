async function getFirebaseConfig() {
    try {
        const response = await fetch('/firebaseconfig/get');
        const config = await response.json();
        return config;
    } catch (error) {
        console.error('Error fetching Firebase config:', error);
        return null;
    }
}

async function initializeFirebase() {
    try {
        const firebaseConfig = await getFirebaseConfig();
        const app = firebase.initializeApp(firebaseConfig);
        const messaging = app.messaging();
        return messaging;
    } catch (error) {
        console.error('Error initializing Firebase:', error);
        return null;
    }
}

async function getToken() {
    togglePreloader(true);
    try {
        const messaging = await initializeFirebase();
        const permission = await Notification.requestPermission();
        if (permission === 'granted') {
            const currentToken = await messaging.getToken({
                vapidKey: 'BPPI3HPUZ0xGI4axfOogJXzKwdOM54c_O9zRGwH_ttz8HXJvv3F4FvMv6-bhzuNJp9ljIgxEQHLKQxdJi-JiX2E'
            });

            if (currentToken) {
                console.log(currentToken);
                return currentToken;
            } else {
                console.log('No registration token available. Request permission to generate one.');
                return "";
            }
        } else {
            console.log('Unable to get permission to notify.');
        }
    } catch (err) {
        console.log('An error occurred while retrieving token. ', err);
        return "";
    } finally {
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

async function startApp() {
    await getToken();
    // Gọi các hàm khác tại đây, nếu có
}

// Gọi hàm async để bắt đầu ứng dụng
startApp();

function deleteToken() {
    messaging.deleteToken().then(() => {
        console.log('Token deleted.');
    }).catch((err) => {
        console.log('Unable to delete token. ', err);
    });
}
