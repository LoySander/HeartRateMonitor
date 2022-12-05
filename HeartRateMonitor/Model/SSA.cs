using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model
{
    public class SSA
    {
        private static MLContext _context = new MLContext();
        private static List<ModelInput> _modelInputs;
        private static SsaForecastingTransformer _forecaster;
        private static TimeSeriesPredictionEngine<ModelInput, ModelOutput> _forecastingEngine;
        private CancellationTokenSource _cancelTokenSource;
        public event PropertyChangedEventHandler PropertyChanged;
        private float _forecastOutput;

        public IEnumerable<ModelInput> ModelInputs
        {
            get;
            set;
        }

        public float ForecastOutput
        {
            get => _forecastOutput;
            set{
                _forecastOutput = value;
                OnPropertyChanged();
            }
        }
        public void ConvertListToModelInput(List<float> rates)
        {
            foreach(var x in rates)
            {
                _modelInputs.Add(new ModelInput { Rate = x });
            }
        }

        private void FitModel()
        {
            if(_modelInputs == null)
            {
                return;
            }
            else
            {
              var pipeline = _context.Forecasting.ForecastBySsa(
              nameof(ModelOutput.RateFuture),
              nameof(ModelInput.Rate),
              windowSize: 5,
              seriesLength: 10,
              trainSize: 50,
              horizon: 1
              );
            _forecaster = pipeline.Fit((IDataView)_modelInputs);
            _forecastingEngine = _forecaster.CreateTimeSeriesEngine<ModelInput, ModelOutput>(_context);
            }

        }
        
        public async Task Fitting()
        {
            _cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancelTokenSource.Token;

            Task task = new Task(() => {
                FitModel();
            }, token);
            task.Start();
            //await task;
        }

        public async Task PredictData(IDataView test)
        {
            _cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancelTokenSource.Token;
           
            Task task = new Task(() => {
                Forecast(test, 1);
            }, token);
            task.Start();
            //await task;
            
        }

        private void Forecast(IDataView testData, int horizon)
        {
            IEnumerable<float> forecastOutput =
            _context.Data.CreateEnumerable<ModelInput>(testData, reuseRowObject: false)
        .Take(horizon)
        .Select((ModelInput rental, int index) =>
        {
            float actualRentals = rental.Rate;
            return actualRentals;
        });
            ForecastOutput = forecastOutput.FirstOrDefault();
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

    }
}
