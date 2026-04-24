package com.example.stargazingapp.network

import com.example.stargazingapp.model.ScoreResponse
import retrofit2.http.GET
import retrofit2.http.Query

interface ApiService {
    @GET("health")
    suspend fun health(): Map<String, Any>

    @GET("api/score")
    suspend fun getScore(
        @Query("lat") lat: Double,
        @Query("lon") lon: Double,
        @Query("hours") hours: Int = 24
    ): ScoreResponse
}
