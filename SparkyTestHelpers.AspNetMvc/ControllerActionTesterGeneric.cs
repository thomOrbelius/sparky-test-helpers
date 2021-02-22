﻿using System;
using System.Web.Mvc;

namespace SparkyTestHelpers.AspNetMvc
{
    /// <summary>
    /// Helper for testing <see cref="Controller"/> actions that return a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">The action response type.</typeparam>
    public class ControllerActionTester<TResponse>
    {
        private readonly Controller _controller;
        private readonly Func<TResponse> _controllerAction;

        /// <summary>
        /// Creates a new <see cref="ControllerActionTester{TResponse}" instance.
        /// </summary>
        /// <param name="controller">The "parent" <see cref="Controller"/>.</param>
        /// <param name="controllerAction">"Callback" function that provides the controller action to be tested.</param>
        public ControllerActionTester(Controller controller, Func<TResponse> controllerAction)
        {
            _controller = controller;
            _controllerAction = controllerAction;
        }

        /// <summary>
        /// Sets up ModelState.IsValid value.
        /// </summary>
        /// <returns>"This" <see cref="ControllerHttpResponseMessageActionTester"/>.</returns>
        public ControllerActionTester<TResponse> WhenModelStateIsValidEquals(bool isValid)
        {
            SetModelStateIsValid(isValid);
            return this;
        }
        
        /// <summary>
        /// Calls controller action, validates <see cref="HttpResponseMessage.StatusCode"/>
        /// (if <see cref="ExpectingHttpStatusCode(HttpStatusCode)" has been called)
        /// and returns the <see cref="HttpResponseMessage"/> returned from the action.
        /// </summary>
        /// <param name="validate">Optional "callback" method to validate the response message.</param>
        /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
        public TResponse Test(Action<TResponse> validate = null)
        {
            TResponse response = _controllerAction();

            validate?.Invoke(response);

            return response;
        }

        private void SetModelStateIsValid(bool isValid = true)
        {
            _controller.ModelState.Clear();

            if (!isValid)
            {
                _controller.ModelState.AddModelError("key", "message");
            }
        }
    }
}
