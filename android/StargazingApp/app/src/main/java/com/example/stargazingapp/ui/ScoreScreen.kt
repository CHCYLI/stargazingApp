package com.example.stargazingapp.ui

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.unit.dp
import androidx.navigation.NavController
import com.example.stargazingapp.viewmodel.ScoreUiState
import com.example.stargazingapp.viewmodel.ScoreViewModel

@Composable
fun ScoreScreen(
    nav: NavController,
    vm: ScoreViewModel
) {
    val state by vm.uiState.collectAsState()

    var latText by remember { mutableStateOf("40.74") }
    var lonText by remember { mutableStateOf("-74.03") }
    var hoursText by remember { mutableStateOf("24") }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp),
        verticalArrangement = Arrangement.spacedBy(12.dp)
    ) {
        Text("Stargazing Score (Phase 3A)", style = MaterialTheme.typography.titleLarge)

        OutlinedTextField(
            value = latText,
            onValueChange = { latText = it },
            label = { Text("Latitude") },
            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
            modifier = Modifier.fillMaxWidth()
        )

        OutlinedTextField(
            value = lonText,
            onValueChange = { lonText = it },
            label = { Text("Longitude") },
            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
            modifier = Modifier.fillMaxWidth()
        )

        OutlinedTextField(
            value = hoursText,
            onValueChange = { hoursText = it },
            label = { Text("Hours") },
            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
            modifier = Modifier.fillMaxWidth()
        )

        Row(horizontalArrangement = Arrangement.spacedBy(10.dp)) {
            Button(onClick = { vm.fetchFixedHoboken() }) {
                Text("Check (Fixed Hoboken)")
            }
            Button(onClick = {
                val lat = latText.toDoubleOrNull()
                val lon = lonText.toDoubleOrNull()
                val hours = hoursText.toIntOrNull() ?: 24
                if (lat != null && lon != null) vm.fetchScore(lat, lon, hours)
            }) {
                Text("Fetch Score")
            }
        }

        when (val s = state) {
            is ScoreUiState.Idle -> Text("Press a button to fetch score")
            is ScoreUiState.Loading -> {
                Row(horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                    CircularProgressIndicator(modifier = Modifier.size(18.dp))
                    Text("Loading...")
                }
            }

            is ScoreUiState.Error -> Text("Error: ${s.message}", color = MaterialTheme.colorScheme.error)
            is ScoreUiState.Success -> {
                val summary = s.data.summary
                Card(modifier = Modifier.fillMaxWidth()) {
                    Column(Modifier.padding(14.dp), verticalArrangement = Arrangement.spacedBy(6.dp)) {
                        Text("Score: ${summary.scoreNow}", style = MaterialTheme.typography.titleMedium)
                        Text("Rating: ${summary.rating}")
                        Text("Bortle: ${summary.bortle} · Moon: ${summary.moonIllumination}%")
                        Spacer(Modifier.height(6.dp))
                        Button(onClick = { nav.navigate("detail") }) {
                            Text("View Details ->")
                        }
                    }
                }
            }
        }
    }
}
