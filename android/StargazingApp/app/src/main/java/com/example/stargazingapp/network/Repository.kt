package com.example.stargazingapp.network

import com.example.stargazingapp.model.ScoreResponse

class Repository(
    private val api: ApiService = RetrofitInstance.api
) {
    suspend fun fetchScore(lat: Double, lon: Double, hours: Int): ScoreResponse {
        return api.getScore(lat, lon, hours)
    }
}
