<script type="module">
  // Import the functions you need from the SDKs you need
  import { initializeApp } from "https://www.gstatic.com/firebasejs/10.7.0/firebase-app.js";
  import { getAnalytics } from "https://www.gstatic.com/firebasejs/10.7.0/firebase-analytics.js";
  // TODO: Add SDKs for Firebase products that you want to use
  // https://firebase.google.com/docs/web/setup#available-libraries

  // Your web app's Firebase configuration
  // For Firebase JS SDK v7.20.0 and later, measurementId is optional
  const firebaseConfig = {
    apiKey: "AIzaSyAXK3Zho4-QwdwKowzrmZVRpBbjxBNDtxw",
    authDomain: "leafcoffee-66175.firebaseapp.com",
    projectId: "leafcoffee-66175",
    storageBucket: "leafcoffee-66175.appspot.com",
    messagingSenderId: "189337812695",
    appId: "1:189337812695:web:57b971e4f3acaf508cba92",
    measurementId: "G-MKB2F9R564"
  };

  // Initialize Firebase
  const app = initializeApp(firebaseConfig);
  const analytics = getAnalytics(app);
</script>