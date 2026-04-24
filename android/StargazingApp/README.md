# Android Phase 3 MVP (for this repo)

This folder contains the minimum Android source skeleton for Phase 3 (Compose + MVVM + Retrofit + Gson), aligned with the backend under `backend/StargazingApi`.

## 1) Create Android Studio project first

In Android Studio:

1. New Project -> Empty Activity (Jetpack Compose) -> Kotlin
2. Use package `com.example.stargazingapp`
3. Create the project at `android/StargazingApp`

After project creation, copy/merge the files in this folder into the generated project.

## 2) Base URL

Use emulator host mapping in `app/src/main/java/com/example/stargazingapp/util/Constants.kt`:

`http://10.0.2.2:5000/`

## 3) Manifest

Ensure this is in `app/src/main/AndroidManifest.xml` (outside `<application>`):

```xml
<uses-permission android:name="android.permission.INTERNET" />
```

## 4) App dependencies

Add these to `app/build.gradle.kts` under `dependencies` (keep your existing Compose BOM/material deps):

```kotlin
implementation("com.squareup.retrofit2:retrofit:2.11.0")
implementation("com.squareup.retrofit2:converter-gson:2.11.0")
implementation("com.squareup.okhttp3:logging-interceptor:4.12.0")
implementation("androidx.lifecycle:lifecycle-viewmodel-compose:2.8.6")
implementation("androidx.navigation:navigation-compose:2.8.0")
```

## 5) Backend checks

Run backend on port `5000`, then verify:

- `GET http://localhost:5000/health` from host machine
- `GET http://10.0.2.2:5000/health` from Android emulator

If using a real device, switch `BASE_URL` to `http://<LAN_IP>:5000/` and make sure backend binds to `0.0.0.0:5000`.
