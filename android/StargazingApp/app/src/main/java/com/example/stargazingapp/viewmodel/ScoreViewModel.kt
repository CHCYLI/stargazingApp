package com.example.stargazingapp.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.stargazingapp.model.ScoreResponse
import com.example.stargazingapp.network.Repository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

sealed class ScoreUiState {
    data object Idle : ScoreUiState()
    data object Loading : ScoreUiState()
    data class Success(val data: ScoreResponse) : ScoreUiState()
    data class Error(val message: String) : ScoreUiState()
}

class ScoreViewModel(
    private val repo: Repository = Repository()
) : ViewModel() {
    private val _uiState = MutableStateFlow<ScoreUiState>(ScoreUiState.Idle)
    val uiState: StateFlow<ScoreUiState> = _uiState

    fun fetchFixedHoboken() {
        fetchScore(lat = 40.74, lon = -74.03, hours = 24)
    }

    fun fetchScore(lat: Double, lon: Double, hours: Int) {
        _uiState.value = ScoreUiState.Loading
        viewModelScope.launch {
            try {
                val res = repo.fetchScore(lat, lon, hours)
                _uiState.value = ScoreUiState.Success(res)
            } catch (e: Exception) {
                _uiState.value = ScoreUiState.Error(e.message ?: "Unknown error")
            }
        }
    }
}
