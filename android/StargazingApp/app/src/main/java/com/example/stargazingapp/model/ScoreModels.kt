package com.example.stargazingapp.model

import com.google.gson.annotations.SerializedName

data class ScoreResponse(
    val location: LocationDto,
    @SerializedName(value = "generatedAt", alternate = ["generated_at"])
    val generatedAt: String,
    val summary: SummaryDto,
    @SerializedName(value = "breakdownNow", alternate = ["breakdown_now"])
    val breakdownNow: List<BreakdownItemDto>,
    val hourly: List<HourlyScoreDto>,
    @SerializedName(value = "bestWindows", alternate = ["best_windows"])
    val bestWindows: List<BestWindowDto>
)

data class LocationDto(
    val lat: Double,
    val lon: Double,
    val name: String
)

data class SummaryDto(
    @SerializedName(value = "scoreNow", alternate = ["score_now"])
    val scoreNow: Int,
    val rating: String,
    val bortle: Int,
    @SerializedName(value = "moonIllumination", alternate = ["moon_illumination"])
    val moonIllumination: Int
)

data class BreakdownItemDto(
    val factor: String,
    val value: Double,
    val penalty: Double,
    val note: String
)

data class HourlyScoreDto(
    val time: String,
    @SerializedName(value = "cloudCover", alternate = ["cloud_cover"])
    val cloudCover: Int,
    @SerializedName(value = "precipProb", alternate = ["precip_prob"])
    val precipProb: Int,
    @SerializedName(value = "windSpeed", alternate = ["wind_speed"])
    val windSpeed: Double,
    val humidity: Int,
    @SerializedName(value = "moonIllumination", alternate = ["moon_illumination"])
    val moonIllumination: Int,
    val bortle: Int,
    val score: Int
)

data class BestWindowDto(
    val start: String,
    val end: String,
    @SerializedName(value = "avgScore", alternate = ["avg_score"])
    val avgScore: Int,
    val reason: String
)
