using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using Microsoft.ML.Data;
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
using System.Collections;

namespace HeartRateMonitor.Model {
    public class SSA {
        private static MLContext _context = new MLContext();
        private static List<ModelInput> _modelInputs = new List<ModelInput>();
        private static SsaForecastingTransformer _forecaster;
        private static TimeSeriesPredictionEngine<ModelInput, ModelOutput> _forecastingEngine;
        private CancellationTokenSource _cancelTokenSource;
        public event PropertyChangedEventHandler PropertyChanged;
        private float _forecastOutput;

        public IEnumerable<ModelInput> ModelInputs {
            get;
            set;
        }

        public float ForecastOutput {
            get => _forecastOutput;
            set {
                _forecastOutput = value;
                OnPropertyChanged();
            }
        }
        public void ConvertListToModelInput(List<float> rates) {
            foreach (var x in rates) {
                _modelInputs.Add(new ModelInput { Rate = x });
            }
        }

        private void FitModel() {
            if (_modelInputs == null) {
                return;
            } else {
                var pipeline = _context.Forecasting.ForecastBySsa(
                nameof(ModelOutput.RateFuture),
                nameof(ModelInput.Rate),
                windowSize: 5,
                seriesLength: 10,
                trainSize: 50,
                horizon: 1
                );
                //_forecaster = pipeline.Fit(new InputObjectDataView(_modelInputs));
                _forecaster = pipeline.Fit(_context.Data.LoadFromEnumerable<ModelInput>(_modelInputs));
                _forecastingEngine = _forecaster.CreateTimeSeriesEngine<ModelInput, ModelOutput>(_context);
            }
            _modelInputs.Clear();
        }

        public async Task Fitting() {
            _cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancelTokenSource.Token;

            Task task = new Task(() => {
                FitModel();
            }, token);
            task.Start();
            //await task;
        }

        public async Task PredictData(List<float> test) {
            _cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancelTokenSource.Token;

            Task task = new Task(() => {
                Forecast(test, 1);
            }, token);
            task.Start();
            //await task;

        }

        private void Forecast(List<float> testData, int horizon) {
            ConvertListToModelInput(testData);
            IEnumerable<float> forecastOutput =
            _context.Data.CreateEnumerable<ModelInput>(_context.Data.LoadFromEnumerable<ModelInput>(_modelInputs), reuseRowObject: false)
        .Take(horizon)
        .Select((ModelInput rental, int index) => {
            float actualRentals = rental.Rate;
            return actualRentals;
        });
            ForecastOutput = forecastOutput.FirstOrDefault();
            _modelInputs.Clear();
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

    }

    public class InputObjectDataView : IDataView {
        private readonly IEnumerable<ModelInput> _data;
        public IEnumerable<ModelInput> Data {
            get {
                return _data;
            }
        }
        public DataViewSchema Schema { get; }
        public bool CanShuffle => false;


        public InputObjectDataView(IEnumerable<ModelInput> data) {
            _data = data;

            var builder = new DataViewSchema.Builder();
            builder.AddColumn("HeartRate", NumberDataViewType.Double);
            Schema = builder.ToSchema();
        }

        public long? GetRowCount() {
            throw new NotImplementedException();
        }

        public DataViewRowCursor GetRowCursor(IEnumerable<DataViewSchema.Column> columnsNeeded, Random rand = null) {
            throw new NotImplementedException();
        }

        public DataViewRowCursor[] GetRowCursorSet(IEnumerable<DataViewSchema.Column> columnsNeeded, int n, Random rand = null) {
            throw new NotImplementedException();
        }
    }
}
