package com.example.stargazingapp

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.example.stargazingapp.ui.DetailScreen
import com.example.stargazingapp.ui.ScoreScreen
import com.example.stargazingapp.viewmodel.ScoreViewModel

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            val nav = rememberNavController()
            val vm: ScoreViewModel = viewModel()

            MaterialTheme {
                Surface {
                    NavHost(navController = nav, startDestination = "score") {
                        composable("score") { ScoreScreen(nav = nav, vm = vm) }
                        composable("detail") { DetailScreen(vm = vm) }
                    }
                }
            }
        }
    }
}
