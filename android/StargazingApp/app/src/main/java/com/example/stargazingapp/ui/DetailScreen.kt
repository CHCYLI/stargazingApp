package com.example.stargazingapp.ui

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.Card
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.example.stargazingapp.viewmodel.ScoreUiState
import com.example.stargazingapp.viewmodel.ScoreViewModel

@Composable
fun DetailScreen(vm: ScoreViewModel) {
    val state by vm.uiState.collectAsState()

    when (val s = state) {
        is ScoreUiState.Success -> {
            val data = s.data
            LazyColumn(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(16.dp),
                verticalArrangement = Arrangement.spacedBy(12.dp)
            ) {
                item {
                    Text("Details", style = MaterialTheme.typography.titleLarge)
                }

                item {
                    Card(Modifier.fillMaxWidth()) {
                        Column(Modifier.padding(14.dp), verticalArrangement = Arrangement.spacedBy(6.dp)) {
                            Text("Summary", style = MaterialTheme.typography.titleMedium)
                            Text("Score: ${data.summary.scoreNow} (${data.summary.rating})")
                            Text("Bortle: ${data.summary.bortle}")
                            Text("Moon: ${data.summary.moonIllumination}%")
                        }
                    }
                }

                item {
                    Text("Breakdown", style = MaterialTheme.typography.titleMedium)
                }
                items(data.breakdownNow) { b ->
                    Card(Modifier.fillMaxWidth()) {
                        Column(Modifier.padding(12.dp)) {
                            Text("${b.factor}  (-${b.penalty.toInt()})", style = MaterialTheme.typography.bodyLarge)
                            Text(b.note, style = MaterialTheme.typography.bodyMedium)
                        }
                    }
                }

                if (data.bestWindows.isNotEmpty()) {
                    item { Text("Best Window", style = MaterialTheme.typography.titleMedium) }
                    items(data.bestWindows) { w ->
                        Card(Modifier.fillMaxWidth()) {
                            Column(Modifier.padding(12.dp)) {
                                Text("${w.start} -> ${w.end}")
                                Text("Avg: ${w.avgScore}", style = MaterialTheme.typography.bodyLarge)
                                Text(w.reason)
                            }
                        }
                    }
                }

                item { Text("Hourly", style = MaterialTheme.typography.titleMedium) }
                items(data.hourly.take(24)) { h ->
                    Card(Modifier.fillMaxWidth()) {
                        Column(Modifier.padding(12.dp)) {
                            Text(h.time)
                            Text("Score ${h.score} · Cloud ${h.cloudCover}% · Rain ${h.precipProb}% · Wind ${h.windSpeed}")
                        }
                    }
                }
            }
        }

        is ScoreUiState.Loading -> {
            Box(Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                CircularProgressIndicator()
            }
        }

        else -> {
            Box(Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                Text("No data yet. Go back and fetch a score first.")
            }
        }
    }
}
